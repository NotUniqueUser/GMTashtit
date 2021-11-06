using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Firestore;
using Java.Interop;
using Plugin.CloudFirestore;
using Exception = System.Exception;
using FieldPath = Plugin.CloudFirestore.FieldPath;
using Object = Java.Lang.Object;

namespace DAL.FIRESTORE
{
    public enum Order_By_Direction
    {
        ACSCENDING,
        DESCENDING
    }

    public abstract class FireStoreDbTable<TEntity, TCollection>
        where TEntity : new() where TCollection : List<TEntity>, new()
    {
        public static async Task<TCollection> SelectAll(string orderBy = "",
            Order_By_Direction order_By_Direction = Order_By_Direction.ACSCENDING)
        {
            IQuerySnapshot query;
            var collection = new TCollection();
            var entityList = new List<TEntity>();

            try
            {
                if (orderBy == "")
                {
                    query = await FireStoreDB.Connection
                        .GetCollection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                        .GetAsync();
                }
                else
                {
                    if (order_By_Direction == Order_By_Direction.ACSCENDING)
                        query = await FireStoreDB.Connection
                            .GetCollection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                            .OrderBy(orderBy, false)
                            .GetAsync();
                    else
                        query = await FireStoreDB.Connection
                            .GetCollection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                            .OrderBy(orderBy, true)
                            .GetAsync();
                }

                entityList = query.ToObjects<TEntity>().ToList();

                if (entityList != null)
                    collection.AddRange(entityList);
            }
            catch (Exception e)
            {
            }

            return collection;
        }

        private void FetchAndListen()
        {
            //FireStoreDB.Connection.Collection("Cities").AddSnapshotListener(this);
        }

        public static async Task<bool> Insert(TEntity entity)
        {
            try
            {
                var docWrapper = (DocumentReferenceWrapper) await FireStoreDB.Connection
                    .GetCollection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                    .AddAsync(entity);

                entity.GetType().GetProperty("IdFs").SetValue(entity, docWrapper.Id);

                //.AddDocumentAsync(entity);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<bool> Update(TEntity entity)
        {
            try
            {
                //string v = entity.GetType().GetProperty("Id").GetValue(entity).ToString();

                //await FireStoreDB.Connection
                //      .GetCollection(COLLECTION_NAME)
                //      .GetDocumentsAsync(v)
                //      .UpdateDataAsync(entity);

                await FireStoreDB.Connection
                    .GetCollection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                    .GetDocument(entity.GetType().GetProperty("IdFs").GetValue(entity).ToString())
                    .UpdateDataAsync(entity);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<bool> Delete(TEntity entity)
        {
            try
            {
                await FireStoreDB.Connection
                    .GetCollection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                    .GetDocument(entity.GetType().GetProperty("IdFs").GetValue(entity).ToString())
                    .DeleteDocumentAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<TEntity> Select(string id)
        {
            try
            {
                var query = await FireStoreDB.Connection
                    .Collection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                    .WhereEqualsTo("Id", id)
                    .GetAsync();

                return query.Count >= 1 ? query.ToObjects<TEntity>().ToList()[0] : default;
            }
            catch (Exception e)
            {
                return default;
            }
        }

        public static async Task<TEntity> Select(string field, string value)
        {
            try
            {
                var query = await FireStoreDB.Connection
                    .Collection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                    .WhereEqualsTo(field, value)
                    .GetAsync();

                return query.Count >= 1 ? query.ToObjects<TEntity>().ToList()[0] : default;
            }
            catch (Exception e)
            {
                return default;
            }
        }

        public static async Task<TCollection> QueryById(string idFs)
        {
            try
            {
                var query = await FireStoreDB.Connection
                    .Collection(typeof(TCollection).Name)
                    .WhereEqualsTo(FieldPath.DocumentId, idFs)
                    .GetAsync();

                var col = new TCollection();
                if (query.Count > 0)
                {
                    col.AddRange(query.ToObjects<TEntity>().ToList());
                    return col;
                }

                return default;
            }
            catch (Exception e)
            {
                return default;
            }
        }

        public static async Task<TCollection> Query(string field, string value)
        {
            try
            {
                var query = await FireStoreDB.Connection
                    .Collection(typeof(TCollection).Name)
                    .WhereEqualsTo(field, value)
                    .GetAsync();

                var col = new TCollection();
                if (query.Count > 0)
                {
                    col.AddRange(query.ToObjects<TEntity>().ToList());
                    return col;
                }

                return default;
            }
            catch (Exception e)
            {
                return default;
            }
        }

        //public static async Task<>

        public void OnEvent(Object obj, FirebaseFirestoreException error)
        {
            throw new NotImplementedException();
        }

        public void SetJniIdentityHashCode(int value)
        {
            throw new NotImplementedException();
        }

        public void SetPeerReference(JniObjectReference reference)
        {
            throw new NotImplementedException();
        }

        public void SetJniManagedPeerState(JniManagedPeerStates value)
        {
            throw new NotImplementedException();
        }

        public void UnregisterFromRuntime()
        {
            throw new NotImplementedException();
        }

        public void DisposeUnlessReferenced()
        {
            throw new NotImplementedException();
        }

        public void Disposed()
        {
            throw new NotImplementedException();
        }

        public void Finalized()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}