/* Copyright (c) 2008, Joern Schou-Rode <jsr@malamute.dk>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE. */

using System;

namespace NCron
{
    /// <summary>
    /// Defines the interface to be implemented by all NCron jobs.
    /// </summary>
    public interface ICronJob : IDisposable
    {
        /// <summary>
        /// Prepares the job before its first execution.
        /// For simple jobs, the body of this method will often be empty.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Executes the job. This method will be called whenever a scheduled execution time for the job is reached.
        /// </summary>
        void Execute();
    }
}
