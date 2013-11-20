Twitter-Quiz
============

Pub quiz that's run over twitter.
Very much "inspired" by NerdTrivia by Laura Masset (@lauralollipop), but tailored to use as a custom quiz.

How it should work
==================

Quiz starts at a set time. The host twitter account tweets a question. Followers DM the host with the answer.
Questions could be tweeted every 10 to 15 minutes depending on your event.
After the last question is tweeted (and after a short break to get more drinks), the host tweets the correct answers 
(also with a set frequency, e.g. 5 minutes) and optionally with some statistics.
Finally, the winner is announced.

Requirements
============

* As an admin, I can set a start date and time
* As an admin, I can set the frequency of questions (which also sets the answer time)
* As an admin, I can set the frequency the correct answers are tweeted (at the end of the quiz)
* As an admin, I can create a new quiz
* As an admin, I can add a question & answer to a quiz
* As an admin, I can add a prize to a quiz
* As an admin, I can add a sponsor twitter handle to a quiz
* As a host, I start the quiz at the given start date and time
* As a player, I can DM the host an answer
* As a host, I can receive DMs from players
* As a host, I can store answers
* As a host, I can flag an answers as correct
* As a host, I can aggregate answers by player
* As a host, I can aggregate answers by question
* As a host, I can determine a winner
* As a host, I can pick a random winner from a tiebreak
* As a host, I can announce the winner (with prizes and sponsor mentions)
