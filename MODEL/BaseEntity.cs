using System;
using System.Collections.Generic;
using Plugin.CloudFirestore.Attributes;
using SQLite;

namespace MODEL
{
    public enum EntityStatus
    {
        ADDED,
        MODIFIED,
        DELETED,
        UNCHANGED
    }

    [Serializable]
    public abstract class BaseEntity
    {
        private EntityStatus entityStatus;
        private int id;
        private string idfs;

        protected BaseEntity() : this(EntityStatus.UNCHANGED)
        {
        }

        protected BaseEntity(EntityStatus entityStatus)
        {
            this.entityStatus = entityStatus;
        }

        [PrimaryKey]
        [AutoIncrement] // SQlite
        [Ignored]
        public int Id
        {
            get => id;
            set => id = value;
        }

        [Id] // FireStore
        [Ignore]
        public string IdFs
        {
            get => idfs;
            set => idfs = value;
        }

        [Ignore] // SQLite
        [Ignored] // FireStore
        public EntityStatus EntityStatus
        {
            get => entityStatus;
            set => entityStatus = value;
        }

        public abstract bool Validate();

        public override bool Equals(object obj)
        {
            return obj is BaseEntity entity &&
                   id == entity.id &&
                   entityStatus == entity.entityStatus;
        }

        public static bool operator ==(BaseEntity left, BaseEntity right)
        {
            return EqualityComparer<BaseEntity>.Default.Equals(left, right);
        }

        public static bool operator !=(BaseEntity left, BaseEntity right)
        {
            return !(left == right);
        }
    }
}