import readlineSync from "readline-sync";

const getInputKey = () => {
	let input;
	do {
		input = readlineSync.question("Enter key: ");
		if (input.length !== 8) {
			console.log("Invalid input, key should be 8 characters long.");
		}
	} while (input.length !== 8);
	return input;
};

const printTablePC1 = (PC1) => {
	let table = "";
	for (let i = 0; i < 8; i++) {
		for (let j = 0; j < 7; j++) {
			table += PC1[i * 7 + j] + " ";
		}
		table += "\n";
	}
	console.log("\nPC1 table:");
	console.log(table);
};

const stringToBinary = (str) => {
	let binaryString = "";
	for (let i = 0; i < str.length; i++) {
		const charCode = str.charCodeAt(i);
		const binaryRepresentation = charCode.toString(2);
		binaryString +=
			"0".repeat(8 - binaryRepresentation.length) + binaryRepresentation;
	}
	return binaryString;
};

const permuteKey = (key) => {
	let permutedKey = PC1.map((index) => key[index - 1]).join("");
	return permutedKey;
};

const PC1 = [
	57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35,
	27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38,
	30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4,
];

const main = () => {
	const key = getInputKey();
	const keyBinary = stringToBinary(key);
	console.log("key binary: ", keyBinary);
	printTablePC1(PC1);
	const permutedKey = permuteKey(keyBinary);
	console.log("permuted key: ", permutedKey);
};

main();
