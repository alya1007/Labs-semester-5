export function containsSpecialCharacters(str) {
	return /[~`!#$%\^&*+=\-\[\]\\';,/{}|\\":<>\?]/g.test(str);
}

export function containsNumbers(str) {
	return /\d/.test(str);
}

export function containsLetters(str) {
	return /[a-zA-Z]/.test(str);
}

export function deleteSpaces(str) {
	return str.replace(/\s/g, "");
}

export function containsSpaces(str) {
	return /\s/.test(str);
}
