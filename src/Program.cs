// Clip.exe reads text into the clipboard and outputs same text to either StdOut or a specified file.
// Copyright( C ) 2025 Timothy J. Bruce

/*
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using Icod.Helpers;
using System.Reflection.PortableExecutable;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Icod.Clip {

	public static class Program {

		private const System.Int32 theBufferSize = 16384;


		[System.STAThread]
		public static System.Int32 Main( System.String[] args ) {
			var processor = new Icod.Argh.Processor(
				new Icod.Argh.Definition[] {
					new Icod.Argh.Definition( "help", new System.String[] { "-h", "--help", "/help" } ),
					new Icod.Argh.Definition( "copyright", new System.String[] { "-c", "--copyright", "/copyright" } ),
					new Icod.Argh.Definition( "input", new System.String[] { "-i", "--input", "/input" } ),
					new Icod.Argh.Definition( "output", new System.String[] { "-o", "--output", "/output" } ),
				},
				System.StringComparer.OrdinalIgnoreCase
			);
			processor.Parse( args );

			if ( processor.Contains( "help" ) ) {
				PrintUsage();
				return 1;
			} else if ( processor.Contains( "copyright" ) ) {
				PrintCopyright();
				return 1;
			}

			System.Func<System.String?, System.Collections.Generic.IEnumerable<System.String>> reader;
			if ( processor.TryGetValue( "input", true, out var inputPathName ) ) {
				if ( System.String.IsNullOrEmpty( inputPathName ) ) {
					PrintUsage();
					return 1;
				} else {
					reader = a => a!.ReadLine();
				}
			} else {
				reader = a => System.Console.In.ReadLine( System.Environment.NewLine );
			}

			System.Action<System.String?, System.Collections.Generic.IEnumerable<System.String>> writer;
			if ( processor.TryGetValue( "output", true, out var outputPathName ) ) {
				if ( System.String.IsNullOrEmpty( outputPathName ) ) {
					PrintUsage();
					return 1;
				} else {
					writer = ( a, b ) => a!.WriteLine( b );
				}
			} else {
				writer = ( a, b ) => System.Console.Out.WriteLine( lineEnding: System.Environment.NewLine, data: b );
			}

			var output = 0;
			try {
				var read = reader( inputPathName! ).ToList().AsReadOnly();
				if ( ( read != null ) && read.Any() ) {
					writer( outputPathName!, read );
					new TextCopy.Clipboard().SetText( System.String.Join( System.Environment.NewLine, read ) );
				} else {
					new TextCopy.Clipboard().SetText( System.String.Empty );
				}
			} catch ( System.Exception e ) {
				System.Console.Out.WriteLine( e.Message );
				output = 1;
			}
			return output;
		}

		private static void PrintUsage() {
			System.Console.Error.WriteLine( "No, no, no! Use it like this, Einstein:" );
			System.Console.Error.WriteLine( "Clip.exe (-h | --help | /help)" );
			System.Console.Error.WriteLine( "Clip.exe (-c | --copyright | /copyright)" );
			System.Console.Error.WriteLine( "Clip.exe [(-i | --input | /input) inputFilePathName] [(-o | --output | /output) outputFilePathName]" );
			System.Console.Error.WriteLine( "Clip.exe reads text into the clipboard and outputs same text to either StdOut or a specified file." );
			System.Console.Error.WriteLine( "inputFilePathName and outputFilePathName may be relative or absolute paths." );
			System.Console.Error.WriteLine( "If inputFilePathName is omitted then input is read from StdIn." );
			System.Console.Error.WriteLine( "If outputFilePathName is omitted then output is written to StdOut." );
		}

		private static void PrintCopyright() {
			var copy = new System.String[] {
				"Clip.exe reads text into the clipboard and outputs same text to either StdOut or a specified file.",
				"Copyright( C ) 2025 Timothy J. Bruce",
				"",
				"This program is free software: you can redistribute it and / or modify",
				"it under the terms of the GNU General Public License as published by",
				"the Free Software Foundation, either version 3 of the License, or",
				"( at your option ) any later version.",
				"",
				"This program is distributed in the hope that it will be useful,",
				"but WITHOUT ANY WARRANTY; without even the implied warranty of",
				"MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the",
				"GNU General Public License for more details.",
				"",
				"You should have received a copy of the GNU General Public License",
				"along with this program.If not, see < https://www.gnu.org/licenses/>."
			};
			foreach ( var line in copy ) {
				System.Console.WriteLine( line );
			}
		}

	}

}