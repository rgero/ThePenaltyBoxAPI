# The Road Map for the Penalty Box

This is the ideal road map for this project, as time progresses I'll update this with checkmarks or adjustments to keep progress clear and driven.

### Implement the desired data models
The existing Penalty Tracker uses a single data model called a "Penalty". These objects are inserted into SQLite tables that are named after the time span that occurred in. It's important to say time span there, because it typically would be "Preseason{YYYY}", "Regular{YYYY}", or "Playoffs{YYYY}". The Penalty itself consisted of the following members

- Player Name
- Player Team
- Penalty
- Opponent Team
- Home/Away Status
- Date
- Referees (this was a list)

While this makes sense, I should dedicate some time to think about whether or not there is a better way of doing it. Then implement it in the project.

### Generate the API
If I'm using ASP.NET, I don't need to have the any script interact directly with the database.

#### Initial thoughts
- The GET should be public, no need for a key
- The POST/PUT functions should be kept behind a key or made private since a single machine should be the only one putting data into the database
- This is going to be the first real dip into ASP.NET testing and will have a little start up cost to learn how it's handled.

### Deploy to Azure
It's important to get a box to play. This doesn't necessarily have to be on Azure and could be my spare computer, but it should be something that is running consistently so I can play with it.

### Modify the Python script to use the API.
This should be a temporary solution just to be able to get data into the database. Originally I was using a cron job to push data into the database.

### Investigate new ways to get data
Once the python script is modified and running, it'd be smart to see if there's other ways to do it.

### Implement the Website Front End
There's a lot that can be done here. While the previous React website worked, and the user was able to search for their desired data, I feel I could provide more search criterias and also make the website run/look a bit smoother.