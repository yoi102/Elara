namespace EventBus.Handlers;

public delegate Task AsyncEventHandler<TEventArgs>(object? sender, TEventArgs e);
