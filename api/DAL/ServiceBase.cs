using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using MyOrm.Common;
using System.ComponentModel;
using DAL.Data;
using MyOrm;
using Castle.Windsor;

namespace DAL.Business
{
    public abstract class ServiceBase
    {
        public static readonly WindsorContainer ServiceContainer = new WindsorContainer();

        protected IServiceFactory Factory { get { return ServiceContainer.Resolve<IServiceFactory>(); } }
    }

    public class ServiceBase<T, TView> : ServiceBase, IEntityService<T>, IEntityViewService<TView>, IEntityService, IEntityViewService
        where TView : T, new()
        where T : EntityBase, new()
    {
        protected virtual CObjectDAO<T> ObjectDAO
        {
            get
            {
                return ServiceContainer.Resolve<ICObjectDAO<T>>() as CObjectDAO<T>;
            }
        }

        private int curId;
        private void ResetId()
        {
            curId = ((CObjectDAO<T>)ObjectDAO).GetMaxID();
        }

        protected virtual CObjectViewDAO<TView> ObjectViewDAO
        {
            get
            {
                return ServiceContainer.Resolve<ICObjectViewDAO<TView>>() as CObjectViewDAO<TView>;
            }
        }

        #region IEntityManager<T> 成员

        public Type EntityType
        {
            get { return typeof(T); }
        }

        public bool Insert(T entity)
        {
            if (InsertCore(entity))
            {
                NotifyEntityChanged(MessageDef.EntityAdded, entity);
                return true;
            }
            else
                return false;
        }

        public bool Update(T entity)
        {
            if (UpdateCore(entity))
            {
                NotifyEntityChanged(MessageDef.EntityUpdated, entity);
                return true;
            }
            else
                return false;
        }

        public bool UpdateOrInsert(T entity)
        {
            switch (UpdateOrInsertCore(entity))
            {
                case UpdateOrInsertResult.Inserted:
                    NotifyEntityChanged(MessageDef.EntityAdded, entity);
                    return true;
                case UpdateOrInsertResult.Updated:
                    NotifyEntityChanged(MessageDef.EntityUpdated, entity);
                    return true;
                default:
                    return false;
            }
        }

        public bool DeleteID(object id)
        {
            T entity = GetObject(id);
            if (entity != null && DeleteIDCore(id))
            {
                NotifyEntityChanged(MessageDef.EntityDeleted, entity);
                return true;
            }
            else
                return false;
        }

        public bool Delete(T entity)
        {
            if (DeleteCore(entity))
            {
                NotifyEntityChanged(MessageDef.EntityDeleted, entity);
                return true;
            }
            return false;
        }

        public int Delete(Condition condition)
        {
            int ret = ObjectDAO.Delete(condition);
            if (ret > 0) NotifyEntityChanged(MessageDef.EntityListChanged, null);
            return ret;
        }

        public void BatchInsert(List<T> entities)
        {
            if (curId <= 0) ResetId();
            foreach (T entity in entities)
            {
                InsertCore(entity);
                curId++;
            }
            NotifyEntityChanged(MessageDef.EntityListChanged, null);
        }

        public void BatchUpdate(List<T> entities)
        {
            foreach (T entity in entities)
            {
                UpdateCore(entity);
            }
            NotifyEntityChanged(MessageDef.EntityListChanged, null);
        }

        public void BatchUpdateOrInsert(List<T> entities)
        {
            foreach (T entity in entities)
            {
                UpdateOrInsert(entity);
            }
            NotifyEntityChanged(MessageDef.EntityListChanged, null);
        }

        public void BatchDelete(List<T> entities)
        {
            foreach (T entity in entities)
            {
                DeleteCore(entity);
            }
            NotifyEntityChanged(MessageDef.EntityListChanged, null);
        }

        public void BatchDeleteID(List<int> ids)
        {
            foreach (object id in ids)
            {
                DeleteIDCore(id);
            }
            NotifyEntityChanged(MessageDef.EntityListChanged, null);
        }

        #endregion

        #region NoNotify Methods

        protected virtual bool InsertCore(T entity)
        {
            if (entity.ID <= 0)
            {
                if (curId <= 0) ResetId();
                entity.ID = curId + 1;
            }
           
            if (ObjectDAO.Insert(entity))
            {
                curId++;
                return true;
            }
            else
                return false;
        }

        protected virtual bool UpdateCore(T entity)
        {
            if (ObjectDAO.Update(entity))
            {
                return true;
            }
            else
                return false;
        }

        protected virtual UpdateOrInsertResult UpdateOrInsertCore(T entity)
        {
            if (entity is BusinessEntity && (entity as BusinessEntity).ID <= 0)
            {
                return InsertCore(entity) ? UpdateOrInsertResult.Inserted : UpdateOrInsertResult.Failed;
            }
            else
            {
                if (ExistsID(entity.ID))
                    return UpdateCore(entity) ? UpdateOrInsertResult.Updated : UpdateOrInsertResult.Failed;
                else
                    return InsertCore(entity) ? UpdateOrInsertResult.Inserted : UpdateOrInsertResult.Failed;
            }
        }

        protected virtual bool DeleteIDCore(object id)
        {
            return ObjectDAO.DeleteByKeys(id);
        }

        protected virtual bool DeleteCore(T entity)
        {
            return ObjectDAO.Delete(entity);
        }
        #endregion

        #region IEntityViewManager<T> 成员

        public Type ViewType
        {
            get { return typeof(TView); }
        }

        public virtual TView GetObject(object id)
        {
            return ObjectViewDAO.GetObject(id);
        }

        public virtual bool ExistsID(object id)
        {
            return ObjectViewDAO.Exists(id);
        }

        public virtual bool Exists(Condition condition)
        {
            return ObjectViewDAO.Exists(condition);
        }

        public virtual int Count(Condition condition)
        {
            return ObjectViewDAO.Count(condition);
        }

        public virtual TView SearchOne(Condition condition)
        {
            return ObjectViewDAO.SearchOne(condition);
        }

        public virtual List<TView> Search(Condition condition)
        {
            return ObjectViewDAO.Search(condition);
        }

        public virtual List<TView> SearchWithOrder(Condition condition, Sorting[] orderBy)
        {
            return ObjectViewDAO.Search(condition, orderBy);
        }

        public virtual List<TView> SearchSection(Condition condition, int startIndex, int sectionSize, string property, ListSortDirection direction)
        {
            SectionSet section = new SectionSet() { StartIndex = startIndex, SectionSize = sectionSize };
            if (!String.IsNullOrEmpty(property)) section.Orders = new Sorting[] { new Sorting() { PropertyName = property, Direction = direction } };
            return ObjectViewDAO.SearchSection(condition, section);
        }

        public virtual List<TView> SearchSection(Condition condition, SectionSet section)
        {
            return ObjectViewDAO.SearchSection(condition, section);
        }

        #endregion

        #region IEntityManager 成员
        bool IEntityService.Insert(object entity)
        {
            return Insert((T)entity);
        }

        bool IEntityService.Update(object entity)
        {
            return Update((T)entity);
        }


        bool IEntityService.UpdateOrInsert(object entity)
        {
            return UpdateOrInsert((T)entity);
        }

        void IEntityService.BatchInsert(IEnumerable entities)
        {
            List<T> list = new List<T>();
            foreach (T entity in entities)
            {
                list.Add(entity);
            }
            BatchInsert(list);
        }

        void IEntityService.BatchUpdate(IEnumerable entities)
        {
            if (entities is List<T>)
                BatchUpdate(entities as List<T>);
            else
            {
                List<T> list = new List<T>();
                foreach (T entity in entities)
                {
                    list.Add(entity);
                }
                BatchUpdate(list);
            }
        }

        void IEntityService.BatchUpdateOrInsert(IEnumerable entities)
        {
            if (entities is List<T>)
                BatchUpdateOrInsert(entities as List<T>);
            else
            {
                List<T> list = new List<T>();
                foreach (T entity in entities)
                {
                    list.Add(entity);
                }
                BatchUpdateOrInsert(list);
            }
        }

        void IEntityService.BatchDelete(IEnumerable entities)
        {
            if (entities is List<T>)
                BatchDelete(entities as List<T>);
            else
            {
                List<T> list = new List<T>();
                foreach (T entity in entities)
                {
                    list.Add(entity);
                }
                BatchDelete(list);
            }
        }

        #endregion

        #region IEntityViewManager 成员

        object IEntityViewService.GetObject(object id)
        {
            return GetObject(id);
        }

        object IEntityViewService.SearchOne(Condition condition)
        {
            return SearchOne(condition);
        }

        IList IEntityViewService.Search(Condition condition)
        {
            return Search(condition);
        }

        IList IEntityViewService.SearchSection(Condition condition, int startIndex, int sectionSize, string property, ListSortDirection direction)
        {
            return SearchSection(condition, startIndex, sectionSize, property, direction);
        }

        IList IEntityViewService.SearchSection(Condition condition, SectionSet section)
        {
            return SearchSection(condition, section);
        }

        IList IEntityViewService.SearchWithOrder(Condition condition, Sorting[] orderBy)
        {
            return SearchWithOrder(condition, orderBy);
        }
        #endregion

        protected void NotifyEntityChanged(MessageDef messageDef, object entity)
        {

        }

    }

    [Serializable]
    public enum OpDef
    {
        Insert,
        Update,
        Delete,
        Search
    }

    [Serializable]
    public enum MessageDef
    {
        EntityAdded,
        EntityUpdated,
        EntityDeleted,
        EntityListChanged,
        Undefined
    }
}
