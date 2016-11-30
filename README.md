# .NET-Master-Slave-task
.Net core training project
Project Description

* Create a user storage system that allows to store users entities.

* A user is an entity with first/last name, date of birth, personal id (like in passport), gender (enum), an array of visa records (struct { country, start, end }). Add functionality to compare users, and get a hashcode of the entity.

* There should be several ways to store users (for ex. In DB), but we need only one implementation – in memory. But there should be a possibility to add an another implementation.

* Methods for storage:

* Add a new user: Add(User user) -> returns User ID

* Search for an user: SearchForUser(ISearchCriteria criteria) -> returns User IDs. At least 3 criteria.

* Possble to use predicate here SearchForUser(Func<T>[] criteria).

* Delete an user: void Delete(...)

* When creating a new there should be a possibility to change the strategy to generate an ID.

* When adding a new user there should be a way to set a different set of rules for validating an user entity before adding it: Add(user) -> validation -> exception if not valid or generate and return a new id.

* Add tests.

* Add an ability to store service state

* A service should have an ability to store it's state in an XML file on disk.

* A service should have an ability to store a last generated ID, so the service can continue generating id from where it stops.

* A filename for XML get from App.config.

* Add an ability to log all requests to a service.

* Use System.Diagnostics.BooleanSwitch for enabling/disabling logging.

* Add a possibility to configure log listeners via App.Config: ConsoleTraceListener, and others.

* Add a replication ability for user service nodes:

* Add an ability to have several instances of user service.

* One service should be Master.

* Other services should be Slaves.

* When Master receives a Add or Delete command, it should send a notification to slave services for updating data.

* If Slave receives Add or Delete command it throws an exception.

* All configuration is stored in App.config: a number of services, their types.

* Create a custom section in App.config file, so the number of instances and their role can be changed there.

* Add functionality that allows master send notifications to slaves via network.

* You will need a new message class that will contain all the data related to event: Add or Delete, User data or id. Make it serializable.

* It's okay if you will establish connection between master and slave when the master node starts.

* Implementations is simpler when master establish connection to slaves, and then maintain it. This implementations assumes that slaves are passive listeners.

* Use NetworkStream to create a channel, and you can use Socket class or TcpListener/TcpClient. (Heorhi said that TcpListener and TcpClient have lack of functionality, and it's simpler to use Socket class. You can consult him if you have any questions).

* It's possible to start slaves first, and then start master to avoid conflicts when master is running and there are no slaves.

* Store all configuration informations about hosts/ports in custom section in App.config.

* Protect your user services from concurrent calls.

* For master – calling Add/Delete, Add/Search, Delete/Search simultaneously.

* For slaves – calling Search and updating an user repository simultaneously.

* Check how your code works by making calls to master service in several threads.

* Make your application run in several threads, one thread per user service. For example:

* T1 is making calls to master service: Add, Delete, Search in cycle with Sleep().

* T2 is making calls to slave #1: Search in cycle with Sleep().

* T3 is making calls to slave #2: Search in cycle with Sleep().

* Changes (Add/Delete) on master node should lead to changes in Search output on slaves.

* Add simple WCF service as a front-end service for master and slaves. * https://msdn.microsoft.com/en-us/library/ms731758(v=vs.110).aspx

* Optimize search operation, because of a huge amount of elements search has O(n), and because of locking collection this may lead to delays.

* Advanced: implement network functionality in asynchronous style (Sockets.Begin...() methods).
