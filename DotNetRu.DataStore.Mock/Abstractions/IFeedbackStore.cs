﻿namespace XamarinEvolve.DataStore.Mock.Abstractions
{
    using System.Threading.Tasks;

    using XamarinEvolve.DataObjects;

    public interface IFeedbackStore : IBaseStore<Feedback>
    {
        Task<bool> LeftFeedback(Session session);
        Task DropFeedback();
    }
}

