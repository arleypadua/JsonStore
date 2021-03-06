﻿using System;
using System.Data;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Dapper;
using SqlKata;
using SqlKata.Compilers;

namespace JsonStore.Sql.Strategies
{
    internal abstract class Strategy
    {
        internal static readonly SqlServerCompiler Compiler = new SqlServerCompiler();

        internal abstract SqlResult GetCommand();

        internal virtual Task Execute(IDbConnection connection, IDbTransaction transaction = null, int commandTimeout = 30)
        {
            var command = GetCommand();

            return connection.ExecuteAsync(
                command.Sql,
                command.NamedBindings,
                transaction,
                commandTimeout);
        }
    }

    internal abstract class Strategy<TDocument, TId, TContent> : Strategy
        where TContent : class
        where TDocument : Document<TContent, TId>, new()
    {
        protected Strategy(Collection<TDocument, TId, TContent> collection, TDocument document)
        {
            Guard.Against.Null(collection, nameof(collection));
            Guard.Against.Null(document, nameof(document));

            CollectionInstance = collection;
            Document = document;
        }

        protected TDocument Document { get; }
        protected Collection<TDocument, TId, TContent> CollectionInstance { get; }
    }
}