# Unreachable code assertions for .NET

The roslyn C# compiler is unfortunately not the brightest when it comes to
detecting that code cannot be reached, and will throw errors in cases like this:

```csharp
enum PossibleValues {
	One,
	Two,
}

...
Foo foo;
if (value == PossibleValues.One) {
	foo = value1;
}
else if (value == PossibleValues.Two) {
	foo = value2;
}

//Refuses to compile with the compiler complaining that foo has a possibly
//undefined value, even though we know that's not the case:
foo.bar();
```

This nuget consists of just the definition for a single exception,
`UnreachableException`. Throwing `UnreachableException` will force the compiler
to let the code through:

```csharp
Foo foo;
if (value == PossibleValues.One) {
	foo = value1;
}
else if (value == PossibleValues.Two) {
	foo = value2;
}
else {
	throw new UnreachableException();
}

//compiles OK this time:
foo.bar();
```

This is no substitute for proper compiler awareness of unreachable code, as if
at some point in the future a new value `PossibleValues.Three` is added to the
enum, the code will still compile and then a `UnreachableException()` will be
thrown at runtime when that branch of the if condition is taken. A proper
compiler would compile the first example above and then refuse to compile it
when `PossibleValues.Three` is added to the enum.

