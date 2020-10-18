# HeeckerenTest
Decoder for HeecherenTest plugin for Procon (Battlefield 3 game)

Install the plugin to Procon layer. It creates a list of files in Heeckeren directory. 
Copy some of them to Debug\Logs directory and run the decoder.

What it is for: To find out the hackers in the game.
The global idea is that a hacker kills more player than they kill him.
For example, there are players A B C D. A killed B and C and D more times than B and C and D killed A.
At the same time B,C,D killed each other similar times. Doesn't matter how much frags it was. We count points:
1 point if positive, -1 if negative and 0 is equal. That means A has 3 points and B,C,D have -1 point each.
And if we divide 3 points of A to victim's quantity (B,C,D) we will have 100% of kills.
The trials shows that normal value we can use is collected after 250kills and it's less than 60% for normal players and 70-100% for hackers.

Some times there can be more than one hacker so they kill each other and lower the percent. But this percent is allways bigger.
The decoder shown the result at the right sorted by points.
