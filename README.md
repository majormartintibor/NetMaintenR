# NetMaintenR

NetMaintenR mimics the use case where a power network needs to be regularly maintained. 
The purpose of this repository is to show how a potentially complex business use case
can be prototyped fast and easy using and showcasing some concepts like Event Sourcing,
Minimal API, A-Frame pattern etc.
It is also used as a Proof of Concept for working with the so called Critter Stack,
mainly utilizing the Wolverine and Marten libraries.

Evolutionary Architecture:
I will try to also show the concept of Evolutionary Architecture.
1.: The first prototype Milestone will only use 1 Web API project with code organized in Feature Folders
following the concept vertical slices.
2.: The second stage might introduce multiple projects and Wolverine's in process message bus as a Mediator tool
3.: The third stage will aim for a microservice architecture using Wolverine in conjuction with RabbitMQ

Feature / Identified Modules

NetObject:
NetObject api is used to create network objects like poles, transformators, cable sections etc.
It might seed the database with initial data.

NetInspectR:
Runs a background service that will cyclically generate network inspection jobs.
The business rule is, that every 6 minutes there will be a small and a large network inspection for a network object.
These will have an offset of 3 minutes, which means that a network object will be inspected every 3 minutes.

NetWorkR:
Manages Workers that can work in the system and do the inspections.

NetJobR:
Matches network inspection jobs with Workers.
TBD: not every worker can do any inspections, there will be some rules how workers and jobs can be matched

NetCloseR:
This is a replacement for (NetForm) the UI for demo purposes. It "fills" out the Form associated to a network inspection job.
It will be done automatically here instead of a manual user input via UI that would be the actual solution
in a production environment.

NetForm:
Not scope of the demo. This would be the UI for the workers to administrate their work on the network inspection.

NetReportR:
For demo purposes. In the first iteration only provides GET endpoints to query all kinds of projections 
built from the evetns streams. Later on a simple UI could be built maybe with some real time updates
using SignalR.

Technical scope:
I plan to keep this as a modular monolith for a long time using a message bus to communicate between modules.
TBD: in process or out of process messaging?
No aothorization/authentication
No secrest etc, appsettings it is
No testing in the first phase. Unit/Integration tests are for phase 2.
No failover handling, just focus on happy path
The point is PROTOTYPING!

Event Streams:
Network objects
Workers?
Cyclic network inspection job

Possible business scope extensions:
A finished network inspection job should trigger maintenance jobs that need to be assigned to specific workers again
and will have their own form.
The cost for doing an inspection or maintanance job should be saved so I can also do financionaly reports.