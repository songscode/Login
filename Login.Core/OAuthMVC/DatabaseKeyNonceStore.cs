using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using DotNetOpenAuth.Messaging.Bindings;
using Login.Core.Data;
using Login.Core.Entity;
using Login.Common;

namespace Login.Core.OAuthMVC
{
    /// <summary>
    /// A database-persisted nonce store.
    /// </summary>
    public class DatabaseKeyNonceStore : INonceStore, ICryptoKeyStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseKeyNonceStore"/> class.
        /// </summary>
        public DatabaseKeyNonceStore()
        {
        }

        #region INonceStore Members

        /// <summary>
        /// Stores a given nonce and timestamp.
        /// </summary>
        /// <param name="context">The context, or namespace, within which the
        /// <paramref name="nonce"/> must be unique.
        /// The context SHOULD be treated as case-sensitive.
        /// The value will never be <c>null</c> but may be the empty string.</param>
        /// <param name="nonce">A series of random characters.</param>
        /// <param name="timestampUtc">The UTC timestamp that together with the nonce string make it unique
        /// within the given <paramref name="context"/>.
        /// The timestamp may also be used by the data store to clear out old nonces.</param>
        /// <returns>
        /// True if the context+nonce+timestamp (combination) was not previously in the database.
        /// False if the nonce was stored previously with the same timestamp and context.
        /// </returns>
        /// <remarks>
        /// The nonce must be stored for no less than the maximum time window a message may
        /// be processed within before being discarded as an expired message.
        /// This maximum message age can be looked up via the
        /// <see cref="DotNetOpenAuth.Configuration.MessagingElement.MaximumMessageLifetime"/>
        /// property, accessible via the <see cref="Configuration"/>
        /// property.
        /// </remarks>
        public bool StoreNonce(string context, string nonce, DateTime timestampUtc)
        {
            try
            {
                var noce = new Nonce { Context = context, Code = nonce, Timestamp = timestampUtc };
                noce.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region ICryptoKeyStore Members

        public CryptoKey GetKey(string bucket, string handle)
        {
            // It is critical that this lookup be case-sensitive, which can only be configured at the database.

            var key = SymmetricCryptoKeyDB.New().Get(bucket, handle);

            return new CryptoKey(Convert.FromBase64String(key.Secret), key.ExpiresUtc.AsUtc());
        }

        public IEnumerable<KeyValuePair<string, CryptoKey>> GetKeys(string bucket)
        {
            var keys = SymmetricCryptoKeyDB.New().Gets(bucket);
            if (keys == null)
                return null;
            return
                keys.Select(
                    e => new KeyValuePair<string, CryptoKey>
                    (e.Handle,new CryptoKey(Convert.FromBase64String(e.Secret), e.ExpiresUtc.AsUtc())));
        }

        public void StoreKey(string bucket, string handle, CryptoKey key)
        {
            var keyRow = new SymmetricCryptoKey()
            {
                Bucket = bucket,
                Handle = handle,
                Secret = Convert.ToBase64String(key.Key),
                ExpiresUtc = key.ExpiresUtc,
            };
            keyRow.Save();
        }

        public void RemoveKey(string bucket, string handle)
        {
            SymmetricCryptoKeyDB.New().Deleted(bucket,handle);
        }

        #endregion
    }
}