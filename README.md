# Negative Currencies

## Getting Started
1. Install the project dependencies - 1.`npm i`. 2 `cd client` -> `npm i`.
2. Run the project server.sln in visual studio.
3. Run the client project by running `npm run dev` from root

You should now have the development version running on your computer and accessible via http://localhost:3000

## description
* I converted the xml to Json in the server to seperate the logic. 
I Created a function that downloads the json to the disk and can be used as an offline task that we can run it (optional: with cache) ,at any time we chose,
in order to prevent from many users to reach the third party url and make an overload traffic.
The reading in react project will be on the file downloaded
