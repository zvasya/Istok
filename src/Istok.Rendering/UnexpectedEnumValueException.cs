namespace Istok;

public class UnexpectedEnumValueException<T>(T value) : Exception("Value " + value + " of enum " + typeof(T).Name + " is not supported");
