using System;

public static class LazyExtensions
{
    public static T AsLazy<T>(this T instance) where T : class
    {
        return new Lazy<T>(() => instance).Value;
    }
}