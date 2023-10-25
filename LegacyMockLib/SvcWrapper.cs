namespace LegacyMockLib;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public class SvcWrapper<T> where T: new() {
    Dictionary<string, Func<object, object[], object>> methods = new ();

    T? instance;
    public T Instance => instance ?? (instance = new T());

    public SvcWrapper(WebApplication app) {
        
    }

    public void RegisterSvcPath(string path, WebApplication app) {
        app.MapGet(path, async context => {
            await context.Response.WriteAsync(typeof(T).Name);
        });
    }

}