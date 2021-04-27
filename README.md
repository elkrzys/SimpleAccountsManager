  # Simple Accounts Manager - web application implementing MVC pattern and .NET technology.
  
  *Application built to learn and improve C# with ASP.NET. Still in development. Code-first approach.*
  
  ## Overview
  
  The purpose is to simplify multiple account management by storing them in a single app. 
  
  There are three view and controller groups:
  - Home, 
  - Content. 
  - Authentication is controlled by Identity.

  Home is a default view for anonymous users.
  
  Content is site (and controller) for user that is signed in - authorization needed.
  
  ## Main features:
  - Data stored in database (currently MS SQL local db),
  - Multi-user application,
  - User Registration and Login controlled by Identity (currently adjusted Login and Registration panels),
  - Anonymous user are not allowed to see Content data,
  - Each signed-in user data displayed with Content controller,
  - User can add new account record, delete or edit one
    - Separate views for *Edit* and *Add*
  - Accounts details:
    - Name, Description, Email, Login, Password
  - Content *pagination* with X.PagedList
  - Passwords are hold in separate table
  - Passwords encrypted with different encryption key for each user,
  - Query id encryption

 Frontend is built using Razor and Bootstrap styling. In future I'm planning to upgrade to better looking Blazor or Angular/React. 
  
