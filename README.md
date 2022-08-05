# MeetupAPI

## Launch requirements:
- Availability of **JetBrains Rider 2022.1.2 IDE**
- Availability of **SDK .NET x64 6.0**

Instruction for start:
1. Download and unpack the solution into a directory that does not contain any Cyrillic symbols.
2. Open the solution in the **Rider IDE** and wait for the complete installation of all the necessary packages.
3. In the terminal, reset the certificates with the following commands:
```shell
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```
4. Build and run all three projects, wait until they are fully loaded.

 **There is adress `https://localhost:7003` by default is the MVC client service**<br> 
 **There is adress `https://localhost:7000/docs` by default is the documentation of the API**
