﻿namespace LetWeCook.Data.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commits all changes made in the current transaction to the database.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The number of state entries written to the database.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="cancellationToken"/> is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
