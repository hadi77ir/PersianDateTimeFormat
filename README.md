# PersianDateTimeFormat

A helper class based on [CoreCLR's DateTimeFormat](https://github.com/dotnet/coreclr/blob/master/src/System.Private.CoreLib/shared/System/Globalization/DateTimeFormat.cs) intended to format DateTime objects into Persian Jalali strings.

## Usage

```c#
string formatted = PersianDateTimeFormat.Format(DateTime.Now, "dd MMMM YYYY", true); // ۲۳ آذر ۱۳۹۷
```

## License

MIT

