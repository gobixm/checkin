﻿namespace Checkin.Common.Domain;

public interface IEntity<TKey>
{
    public TKey Id { get; set; }
}