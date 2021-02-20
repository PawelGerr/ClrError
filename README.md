# Repro for `Fatal error. Internal CLR error.`

* Execute the project `ClrError`
* The application starts an ASP.NET Core application, creates a SQLite database `test.db` and populates it with data
* After previous setup of the test environment, the application starts doing POST requests in a loop
* Usually, after 10-20 iterations the process crashes.   
  One of the following messages are displayed in console:
    * CLR error and exit code
        ```
        Fatal error. Internal CLR error. (0x80131506)

        Process finished with exit code -1,073,741,819.
        ```
    * Or just the exit code
      ```
      Process finished with exit code -1,073,741,819.
      ```
    * There have a few occasions when an `ArgumentNullException` is thrown - in this case repeat the execution of the app
    

The solution is to change `private struct Culprit` to a `class`, i.e., to `private class Culprit`.
