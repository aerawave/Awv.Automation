# Awv.Automation

Awv.Automation is a set of libraries used for automating certain processes, mostly for use with bots and random generation.

Below are the current libraries included in this solution, and what they do:

## Awv.Automation.Generation
---

Awv.Automation.Generation is used as a foundation for randomly generated objects and values. It all goes back to two core interfaces: [IRNG](Awv.Automation.Generation/Interface/IRNG.cs) and [IGenerator](Awv.Automation.Generation/Interface/IGenerator.cs).

IRNG is essentially an interface developed to wrap the default System.Random class. The actual implementation of this can be found in the [RNG](Awv.Automation.Generation/RNG.cs) class.

The primary reason this was created was to facility reproducing values (with or without a seed provided), via something such as the [Sequence](Awv.Automation.Generation/Sequence.cs) class.

That can then be encoded to base64 to be saved to wherever, then loaded back whenever to reproduce the same outputs. This can also be used if you want to provide explicit values for the IRNG interface.

### Usage Example

``` C#
// The below code is pretty useless other than for demonstrating usage. It just generates a random System.IO.SeekOrigin value.
IRNG random = new RNG();
var generator = new EnumGenerator<System.IO.SeekOrigin>(); // This inherits from PredefinedGenerator, which itself implements IGenerator. See below for links.

var randomSeekOrigin = generator.Generate(random);

// Do whatever with your randomly generated SeekOrigin...
```
Referenced interfaces/classes:
- [IRNG](Awv.Automation.Generation/Interface/IRNG.cs)
- [RNG](Awv.Automation.Generation/RNG.cs)
- [EnumGenerator<TEnumType>](Awv.Automation.Generation/EnumGenerator.cs)
- [PredefinedGenerator<TGenerationType>](Awv.Automation.Generation/PredefinedGenerator.cs)
- [PredefinedGenerator<TGenerationType>](Awv.Automation.Generation/PredefinedGenerator.cs)
- [IGenerator<TGenerationType>](Awv.Automation.Generation/Interface/IGenerator.cs)

## Awv.Automation.Lexica
---

Awv.Automation.Lexica is essentially an extension to the [Awv.Lexica](https://www.nuget.org/packages/Awv.Lexica/) library which adds some more customized modifications to it that suit the needs of the Awv.Automation set of libraries.

It essentially serves the purpose to implement the [Phrase](Awv.Automation.Lexica/Phrase.cs) class (with tags), as well as its respectvie [PhraseGenerator](Awv.AUtomation.Lexica/PhraseGenerator.cs) generator.

Additionally, to utilize this generator (with tags), and to implement a conditional Lexigram to Awv.Lexica, the [TagLexigram](Awv.AUtomation.Lexica/Compositional/Lexigrams/TagLexigram.cs) and [ConditionalLexigram](Awv.AUtomation.Lexica/Compositional/Lexigrams/ConditionalLexigram.cs) are used respectively. These are instantiated in the [AutomationParser](Awv.AUtomation.Lexica/Compositional/AutomationParser.cs), which is an extension of the CompositionParser from Awv.Lexica.

## Usage Example

``` c#
class ExampleLibrary : ILibrary
{
    public static IRNG Random { get; set; }
    public static PhraseGenerator Phrases { get; set; }
    public static bool calculate_chance(double chance)
    {
        return Random.NextDouble() < chance;
    }

    public static string tag(string tag)
    {
        return Phrases.Tagged(tag).Generate(Random).Value;
    }

    public static int ri(int min, int max) => randomi(min, max);

    public static int randomi(int min, int max) => Random.Next(min, max);
}

class Program
{
    static void Main(string[] args)
    {
        var phrases = new List<Phrase>();

        var element = new string[] { "Fire", "Shock", "Flame", "Fel", "Light", "Wind" }.Select(p => new Phrase(p)).ToArray();
        foreach(var p in element)
        {
            p.Tag("element");
            phrases.Add(p);
        }

        var rage = new string[] { "Rage", "Fury", "Havoc" }.Select(p => new Phrase(p)).ToArray();
        foreach (var p in rage)
        {
            p.Tag("rage");
            phrases.Add(p);
        }

        ExampleLibrary.Random = new RNG();
        ExampleLibrary.Phrases = new PhraseGenerator(phrases);
        
        var name = new AutomationParser("{{#element(element)#rage}(name), Blessed Blade of the #element(element2)seeker}(fullname)").Transpile();
        var chanceOnHit = new AutomationParser("`name` hurls a ball of `element` at the target, dealing `ri(400,2800)` `element2` damage.").Transpile();
        
        Console.WriteLine($"Input name: {name}");
        Console.WriteLine($"Input chanceOnHit: {chanceOnHit}");
        Console.WriteLine();
        Console.WriteLine();
        for (var i = 0; i < 5; i++)
        {
            var engine = new CompositionEngine();
            engine.RegisterLibrary<ExampleLibrary>();
            Console.WriteLine($"Example {i + 1}:");
            Console.WriteLine($"\tName: {name.Build(engine)}");
            Console.WriteLine($"\tChance on Hit: {chanceOnHit.Build(engine)}");
        }
        Console.ReadLine();
    }
}

```