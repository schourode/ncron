/*
 * Copyright 2009, 2010 Joern Schou-Rode
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

namespace NCron.Logging
{
    /// <summary>
    /// Defines the interface of a log to which cron jobs can write messages of different severity levels.
    /// </summary>
    /// <remarks>
    /// All methods in this interface takes only delegate typed parameters, allowing implementers to opt out of parameters by not invoking the delegates.
    /// If an implementation is designed to ignore messages of a specific severity level, there is no need to do expensive string manipulation (for the error message) or build a stack trace (for an exception).
    /// </remarks>
    public interface ILog : IDisposable
    {
        /// <summary>
        /// Writes a "debug" level message to the log.
        /// </summary>
        /// <param name="msgCallback">The message to be written to the log.</param>
        void Debug(Func<string> msgCallback);

        /// <summary>
        /// Writes a "debug" level message to the log, including detailed information from an <see cref="Exception"/> object.
        /// </summary>
        /// <param name="msgCallback">The message to be written to the log.</param>
        /// <param name="exCallback">The exception that triggered this log message.</param>
        void Debug(Func<string> msgCallback, Func<Exception> exCallback);

        /// <summary>
        /// Writes a "info" level message to the log.
        /// </summary>
        /// <param name="msgCallback">The message to be written to the log.</param>
        void Info(Func<string> msgCallback);

        /// <summary>
        /// Writes a "info" level message to the log, including detailed information from an <see cref="Exception"/> object.
        /// </summary>
        /// <param name="msgCallback">The message to be written to the log.</param>
        /// <param name="exCallback">The exception that triggered this log message.</param>
        void Info(Func<string> msgCallback, Func<Exception> exCallback);

        /// <summary>
        /// Writes a "warn(ing)" level message to the log.
        /// </summary>
        /// <param name="msgCallback">The message to be written to the log.</param>
        void Warn(Func<string> msgCallback);

        /// <summary>
        /// Writes a "warn(ing)" level message to the log, including detailed information from an <see cref="Exception"/> object.
        /// </summary>
        /// <param name="msgCallback">The message to be written to the log.</param>
        /// <param name="exCallback">The exception that triggered this log message.</param>
        void Warn(Func<string> msgCallback, Func<Exception> exCallback);

        /// <summary>
        /// Writes a "error" level message to the log.
        /// </summary>
        /// <param name="msgCallback">The message to be written to the log.</param>
        void Error(Func<string> msgCallback);

        /// <summary>
        /// Writes a "error" level message to the log, including detailed information from an <see cref="Exception"/> object.
        /// </summary>
        /// <param name="msgCallback">The message to be written to the log.</param>
        /// <param name="exCallback">The exception that triggered this log message.</param>
        void Error(Func<string> msgCallback, Func<Exception> exCallback);
    }
}
