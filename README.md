# GetSwift code test
This is my implementation of [the code test by GetSwift](https://github.com/GetSwift/codetest)

The test is centered around two endpoints of an API: A list of delivery drones and a list of packages to be delivered. The task is to take input from the two lists assign a drone to deliver each package to its destination by its deadline.

# How did you implement your solution and why do it this way?
For this test, I created a console application adopting a service-based approach using C# and the .NET Core framework. No third party tools, packages or frameworks were used. In doing this, I am ensuring portability and compatibility of the service across all platforms (Windows,MacOS, Linux). Installing and running the service is a matter of cloning the repo then running [code]dotnet restore[/code],[code]dotnet build[/code],[code]dotnet run[/code].

The entry point point to the application [Program.cs](https://github.com/Briscoooe/Swift/Program.cs), is where all of the application's dependencies are registered and resolved using an IoC (Inversion of Control) container. Using an IoC container makes the code easily maintainable as it enables you to freely alter the dependencies of a class and only need to change it in one place. Furthermore, using dependency injection makes each component loosely coupled and easily testable.

Complex objects are registered as interfaces and configurations are registered as a custom [AppSettings](https://github.com/Briscoooe/Swift/AppSettings.cs) class. The AppSettings class represents the structure JSON file containing configuration values for the service, which are loaded at startup. Loading these values from an external file, rather than hardcoding them into the service, provides the freedom to easily change these values without needing to recompile the service. 

The App [App.cs](https://github.com/Briscooe/Swift/App.cs) class contains an IDeliveryService interface. Currently the class does nothing more than stop and start the IDeliveryService. However, the class is easily extendable as any number of services could be injected into the App, for example an ICustomerService or IPaymentService.

The IDeliveryService interface is implemented by the [DeliveryService](https://github.com/Briscooe/Swift/DeliveryService.cs) class. This class takes an IRequestSender and IJourneyCalculator as constructor parameters. The class only contains two public methods, [code]Start()[/code] and [code]Stop()[/code]. By wrapping all operations inside these methods, any number of operations can be added to the service, such as the[code]AssignDrones()[/code]method, and any external caller need not know the difference. 

The IRequestSender interface is implemented by the [RequestSender](https://github.com/Briscooe/Swift/RequestSender.cs) class. This class takes an API base URL and contains one method, "GetObjects<T>". By using generics, this class can be used to return a list of any object type from any API that returns JSON. It could of course be extended to support different data types. In this service it is used to return a list of Drone objects from the "drones" endpoint and a list of Packages from the "packages" endpoint.

The [JourneyCalculator](https://github.com/Briscooe/Swift/JourneyCalculator.cs) implements the IJourneyCalculator, required by the DeliveryService. The JourneyCalculator performs all calculations regarding the time taken to get from one coordinate to the other. It is the only part of the system that knows about the speed of the drone and the location of the depot, both of which are passed into the constructor. 

## Algorithm
The [code]AssignDrones()[/code] method starts by retrieving the list of packages then the list of drones, each sorted using LINQ.

The packages are ordered by which need to leave the depot soonest (deadline minus time to deliver package). It is important that this is the sort order, rather than simply by deadline, as it considers the fact that while some packages have later deadlines than others, they may take longer to get to and as a result must be handled first.

The drones are sorted by shortest trip back to the depot. This is calculated by adding the time to deliver it's current package (if any) and returning back to the depot.

The next step is to iterate over each package. For each package, the distance from the depot to its destination is calculated. Within this loop, each package that has not already been assigned is iterated over. The total time for the journey is calculated by adding the time it will for the given drone to return to the depot to the time it will take for the drone to reach the packages destination from the depot. Finally, the package deadline is subtracted from the total time. If this value is greater than the current time, the package cannot be delivered and the package ID is added to a list of unassigned packages. Otherwise, a new assignment is created using the current package ID and the current drone ID. The loop then "break"'s either way. 

The advantage of pre-sorting the two lists before iterating over them is that the inner loop over the drones only executes once. This is because the first drone on the list will always be the one that can deliver the package soonest. If this one cannot deliver the package on time, none of the remaining ones will. 

Finally, the results are printed to the console and the program finishes.

# Scalability 
The core logic of the solution would still run fine with thousands of requests per second, however a few changes need to be made to the architecture. It would be more appropriate to run the app as a multithreaded background service rather than a console application. 

The service would run in a continuous loop. After an interval has expired, e.g. 1 second, the main pipeline, as seen in the current solution, would run on a new thread, which would then be correctly disposed of after. It is important to configure a maximum number of threads in a situtation like this. 

To make it even more scalable, the service could be deployed in multiple instances or across multiple servers. Each service/server would communicate with a global cache, e.g. Redis, which would hold a master list of package and drone assignments. When the drones and packages are retrieved, logic would be added to specifically ignore any package or drone currently assigned in this cache. This way none of the assignments would be conflicting