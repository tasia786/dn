# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved

Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?

I moved R7. It was in Services/BookingService.cs and it used <=. Now it is in Domain/Bookings/Booking.cs and uses < (strict inequality) and counts only the active bookings.
I moved it there because the rule is about the booking itself, so it lives in the domain; the rule now cannot be bypassed, a state is always a valid state.

## 2. Result instead of an exception

Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?

A room being occupied is an expected business outcome, not an unexpected failure. With result, it is clear from the signature that the method can fail. Additionaly, the expections are slower.

When an error is returned, the handler sees IsFailure and returns an error.

## 3. Who knows about EF Core?

Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

Infrastructure/Persistence, know about EF Core, and Domain and Features don't.
This approach is beneficial because the domain stays clean and I could always swap the database with ease, changing the database would not break the code

The downside is that there are more files and interface

## 4. The flow of a request

Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

_(your answer here)_

## 5. What you rejected from what the AI gave you

Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

_(your answer here)_

## 6. If you had 30 more minutes

What would you improve and what risk did you leave in the project?

_(your answer here)_
