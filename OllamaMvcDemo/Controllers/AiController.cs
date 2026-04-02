using Microsoft.AspNetCore.Mvc;
using OllamaMvcDemo.Services;

namespace OllamaMvcDemo.Controllers;

public class AiController : Controller
{
    private readonly OllamaService _ollama;

    public AiController(OllamaService ollama)
    {
        _ollama = ollama;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Ask(string prompt)
    {
        var result = await _ollama.AskAsync(prompt);

        ViewBag.Response = result;

        return View("Index");
    }
}