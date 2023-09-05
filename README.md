# Codenames Solver
A Blazor Server app for finding good 'Codewords' for the game Codenames

C# is relatively new to me, and Blazor is totally new - this was an interesting project I took on to learn more about NLP, dotnet and C#.

The rules of the game are for a codemaster to provide a number N and a 'Codeword' that relates to N words
on the board, the rest of the team must try to guess these related words, while avoiding unrelated neutral,
opposing, and assassin words.

## NLP logic:

We're using a Word2vec model - a method of embedding a text corpus in a vector space, allowing us
to perform similarity searches, generate representaitons of multiple words, and do some other cool things.

The general algorithm for generating codewords is as follows:

1. Create a board state, specify the number of related words desired, specify the team colour - send this to a controller.

2. Generate all permutations of words from your team's remaining cards.

3. Add the words in each permutation together to form a vector representation.

4. Find the nearest words to those representations (these are our candidate codewords).

5. For each candidate, compute its initial score from its 'distance' to the representation in step 3 multiplied by a positive coefficient.

6. From the initial score, deduct points for the cosine similarity between 'candidate' and neutral, opposing, and assassin card(s).

7. Select a shortlist of good codewords for each permutation.

8. Select a shortlist of best permutations based on their best codewords.

## Other cool stuff I learned about

How to implement a fuzzy searching service for C#.

How blazor server works

How to listen to state changes between multiple components - syncrhonising two separate components

## To be implemented

Inverse problems: assessing codewords & figuring out the words from a codeword

Better namespace segregation

More closing following SOLID principles
