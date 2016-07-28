﻿namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class UnitOfWorkGeneration : Idea.UnitOfWork.UnitOfWorkGeneration
    {
        protected override void Release(IUnitOfWork uow)
        {
            var current = uow as UnitOfWork;
            if (current != null)
            {
                current.Release();
            }
        }
    }
}