# Proteo5.HL
This library contains reusable helpers along .net core 5 projects

## Nuget Install 

```sh
PM> Install-Package Proteo5.HL 
```

[Nuget Proteo5.HL](https://www.nuget.org/packages/Proteo5.HL/)

## Helpers

- **Result**: Facilitate the comunication between appliction layers and services.
- **TokenHL**: Create and validate JWT tokens.
- **ShortCodes**: Create code from long integers and get back the original number from the code.
- **HashHL**: Generates Hashs among MD5, SHA1, SHA256, SHA384 and SHA512.
- **FileHL**: Helps Upload files for ASP.NET Controllers.
- **EmailHL**: Helps send emails using SMTP.
- **EnvironmentsHL**: Alternative way to handle connection strings on an application.

## Validator

Validator used with Result to ensure the proper validation of the data input and a detailed description sended back to the caller. 
