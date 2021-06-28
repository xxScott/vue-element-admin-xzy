using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using MyOrm.Common;
using System.ComponentModel;

namespace DAL.Business
{
    [Service]
    [ServicePermission(false)]
    public interface IEntityService<T> : IEntityService
    {
        bool Insert(T entity);

        bool Update(T entity);

        bool UpdateOrInsert(T entity);

        [Transaction]
        void BatchInsert(List<T> entities);

        [Transaction]
        void BatchUpdate(List<T> entities);

        [Transaction]
        void BatchUpdateOrInsert(List<T> entities);

        [Transaction]
        void BatchDelete(List<T> entities);
    }

    [ServicePermission(false)]
    public interface IEntityService
    {
        bool Insert(object entity);

        bool Update(object entity);

        bool UpdateOrInsert(object entity);

        bool DeleteID(object id);

        [Transaction]
        void BatchInsert(IEnumerable entities);

        [Transaction]
        void BatchUpdate(IEnumerable entities);

        [Transaction]
        void BatchUpdateOrInsert(IEnumerable entities);

        [Transaction]
        void BatchDelete(IEnumerable entities);

        [Service]
        [Transaction]
        void BatchDeleteID(List<int> ids);
    }

    [Service]
    [ServicePermission(true)]
    [ServiceLog(LogLevel = LogLevel.Debug)]
    public interface IEntityViewService<T> : IEntityViewService
    {
        new T GetObject(object id);

        new T SearchOne(Condition condition);

        new List<T> Search(Condition condition);

        new List<T> SearchWithOrder(Condition condition, Sorting[] orderBy);

        new List<T> SearchSection(Condition condition, int startIndex, int sectionSize, string property, ListSortDirection direction);

        [Service(Name="SearchSection2")]
        new List<T> SearchSection(Condition condition, SectionSet section);
    }

    [ServicePermission(true)]
    [ServiceLog(LogLevel = LogLevel.Debug)]
    public interface IEntityViewService
    {
        object GetObject(object id);

        [Service]
        bool ExistsID(object id);

        [Service]
        bool Exists(Condition condition);

        [Service]
        int Count(Condition condition);

        object SearchOne(Condition condition);

        IList Search(Condition condition);

        IList SearchWithOrder(Condition condition, Sorting[] orderBy);

        IList SearchSection(Condition condition, int startIndex, int sectionSize, string property, ListSortDirection direction);

        IList SearchSection(Condition condition, SectionSet section);
    }
}
