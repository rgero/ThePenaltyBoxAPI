# The Penalty Box

This project is going to be a revisit to the concept of the Penalty Tracker. Originally, that project consisted of three different components.

1. A Python script that exercised the NHL's public API to obtain the necessary data and publish it to a SQLite database
2. A React website that interacted allowed the user to fill out a form and obtain their desired results from the database
3. A Node API that was intended on allowing the user to fill out the criteria without going through the website.

This worked fine and exercised my knowledge in different areas, but having a repository for each aspect made deploying it to my server cumbersome. This annoyance made maintaining and expanding the project more difficult.

That's where the Penalty Box comes in.

## Overview of the Penalty Box

I'm intending on rewriting the whole project in C# using ASP.NET Core 6.0, and ideally will have the same functionality as the previous project, but in a one stop shop rather than pieced together. I'm going to outline the process in "Roadmap.md"

The ultimate goal will be for me to exercise my knowledge of ASP.NET to develop a full application, with tests and deploy it to Azure before the start of the 2022-23 NHL pre-season (which is probably around the middle of September)