import readlineSync from "readline-sync";
import {
	containsSpecialCharacters,
	containsNumbers,
	containsLetters,
	containsSpaces,
	deleteSpaces,
} from "./helpers.js";

function getInput(prompt, validate) {
	let input;
	do {
		input = readlineSync.question(prompt);
		if (!validate(input)) {
			console.log("Invalid input. Please try again.");
		}
	} while (!validate(input));
	return input;
}

function getOperation() {
	return getInput(
		"Enter operation (e/d): ",
		(input) => input === "e" || input === "d"
	);
}

function getNumberOfKeys() {
	return getInput(
		"Enter number of keys (1/2): ",
		(input) => input === "1" || input === "2"
	);
}

function getText() {
	return getInput(
		"Enter text: ",
		(text) =>
			text.length > 0 &&
			!containsSpecialCharacters(text) &&
			!containsNumbers(text)
	);
}

function getKey() {
	return getInput(
		"Enter key: ",
		(key) =>
			key.length > 0 &&
			!containsLetters(key) &&
			parseInt(key) >= 1 &&
			parseInt(key) <= 25
	);
}

function getSecondKey() {
	return getInput(
		"Enter second key: ",
		(key) =>
			key.length > 6 &&
			!containsNumbers(key) &&
			!containsSpecialCharacters(key) &&
			!containsSpaces(key)
	);
}

function getIndexes(text, alphabet) {
	return text.split("").map((letter) => alphabet.indexOf(letter));
}

function encrypt(text, key, alphabet) {
	const indexes = getIndexes(text, alphabet);
	const encryptedIndexes = indexes.map((index) => (index + parseInt(key)) % 26);
	return encryptedIndexes.map((index) => alphabet[index]).join("");
}

function decrypt(text, key, alphabet) {
	const indexes = getIndexes(text, alphabet);
	const decryptedIndexes = indexes.map((index) => {
		if (index !== -1) {
			let newIndex = (index - parseInt(key)) % 26;
			if (newIndex < 0) {
				newIndex = 26 + newIndex;
			}
			return newIndex;
		}
	});
	return decryptedIndexes
		.map((index) => (index !== undefined ? alphabet[index] : ""))
		.join("");
}

function insertSecondKey(secondKey, alphabet) {
	var alphabetWithSecondKey = (secondKey + alphabet).split("");
	return [...new Set(alphabetWithSecondKey)];
}

function main() {
	var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	const operation = getOperation();
	const numberOfKeys = getNumberOfKeys();
	const text = deleteSpaces(getText().toUpperCase());
	const key = getKey();

	if (numberOfKeys === "2") {
		const secondKey = getSecondKey().toUpperCase();
		alphabet = insertSecondKey(secondKey, alphabet).join("");
	}

	if (operation === "e") {
		console.log(encrypt(text, key, alphabet));
	} else {
		console.log(decrypt(text, key, alphabet));
	}
}

main();
