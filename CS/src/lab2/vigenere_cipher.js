import readlineSync from "readline-sync";
import {
	containsSpecialCharacters,
	containsNumbers,
	deleteSpaces,
} from "../lab1/helpers.js";

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
			key.length > 4 && !containsNumbers(key) && !containsSpecialCharacters(key)
	);
}

function getIndexes(text, alphabet) {
	return text.split("").map((letter) => alphabet.indexOf(letter));
}

function encrypt(text, key, alphabet) {
	const textIndexes = getIndexes(text, alphabet);
	const keyIndexes = getIndexes(key, alphabet);

	const encryptedIndexes = [];
	for (let i = 0; i < textIndexes.length; i++) {
		const textIndex = textIndexes[i];
		const keyIndex = keyIndexes[i % keyIndexes.length];
		const encryptedIndex = (textIndex + keyIndex) % alphabet.length;
		encryptedIndexes.push(encryptedIndex);
	}

	const encryptedText = encryptedIndexes
		.map((index) => alphabet[index])
		.join("");
	return encryptedText;
}

function decrypt(text, key, alphabet) {
	const textIndexes = getIndexes(text, alphabet);
	const keyIndexes = getIndexes(key, alphabet);

	const decryptedIndexes = [];
	for (let i = 0; i < textIndexes.length; i++) {
		const textIndex = textIndexes[i];
		const keyIndex = keyIndexes[i % keyIndexes.length];
		const decryptedIndex =
			(textIndex - keyIndex + alphabet.length) % alphabet.length;
		decryptedIndexes.push(decryptedIndex);
	}

	const decryptedText = decryptedIndexes
		.map((index) => alphabet[index])
		.join("");
	return decryptedText;
}

function main() {
	var alphabet = "AĂÂBCDEFGHIÎJKLMNOPQRSȘTȚUVWXYZ";

	const operation = getOperation();
	const text = deleteSpaces(getText().toUpperCase());
	const key = deleteSpaces(getKey().toUpperCase());

	if (operation === "e") {
		console.log(encrypt(text, key, alphabet));
	} else {
		console.log(decrypt(text, key, alphabet));
	}
}

main();
