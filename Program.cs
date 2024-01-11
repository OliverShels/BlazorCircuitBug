using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(x => x.DetailedErrors = true);

// BUG
// Including this line causes the client to hang up and print the message 'Error: The circuit failed to initialize' to
// the browser console. No extra debug information is included, even with 'DetailedErrors' turned on in CircuitOptions.
//
// On the server side, there are no exceptions printed. There *is* an InvalidOperationException sent to the logs, but
// it is of severity 'Debug', which, in my opinion, is too low.
//
// My suggestion: Either:
//  - Print a higher severity log message if a circuit fails to initialize due to unresolved dependencies, OR;
//  - Include information on the client side (browser console) saying why the circuit failed to initialize, when
//    'DetailedErrors' are on.
// Ideally, both of these!
builder.Services.AddScoped<CircuitHandler, MyBadCircuitHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


public interface IMyUnresolvedDependency
{

}

public class MyBadCircuitHandler : CircuitHandler
{
    public MyBadCircuitHandler(IMyUnresolvedDependency dep)
    {

    }
}
