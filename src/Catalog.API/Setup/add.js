const fs = require('fs');
const path = require('path');

// Path to the input file
const inputFile = path.join(__dirname, 'catalog.json');
// Path to the output file
const outputFile = path.join(__dirname, 'output.json');

// Function to add a rating after each price
function addRatingToFile() {
    fs.readFile(inputFile, 'utf8', (err, data) => {
        if (err) {
            console.error('Error reading file:', err);
            return;
        }

        // Split the data into lines
        const lines = data.split('\n');
        let outputLines = [];

        // Process each line
        lines.forEach(line => {
            outputLines.push(line); // Add the original line to output

            // Check if the line contains "Price"
            if (line.trim().startsWith('"Price":')) {
                // Generate a random number between 0 and 3
                const randomRating = Math.floor(Math.random() * 4);

                // Create the rating line
                const ratingLine = `    "Rating": ${randomRating},`;

                // Add the rating line right after the price line
                outputLines.push(ratingLine);
            }
        });

        // Join the output lines back into a single string
        const outputData = outputLines.join('\n');

        // Write the modified data to the output file
        fs.writeFile(outputFile, outputData, 'utf8', (err) => {
            if (err) {
                console.error('Error writing file:', err);
                return;
            }
            console.log('File has been successfully updated with ratings.');
        });
    });
}

// Call the function to process the file
addRatingToFile();

