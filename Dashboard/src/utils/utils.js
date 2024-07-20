import AxiosRequester from "../Axios/AxiosRequester.js";

export const getBase64 = (file) => {
	return new Promise((resolve, reject) => {
		const reader = new FileReader();
		reader.readAsDataURL(file);
		reader.onload = () => {
			let encoded = reader.result.toString().replace(/^data:(.*,)?/, "");
			if (encoded.length % 4 > 0) {
				encoded += "=".repeat(4 - (encoded.length % 4));
			}
			resolve(encoded);
		};
		reader.onerror = (error) => reject(error);
	});
};

export const isValidEmail = (email) => {
	const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
	return regex.test(email);
};

export const getRandomInt = (number) =>{
	// min and max are inclusive
	return (number % 7) +1;
}

export const isEmailAvailable = async (email) => {
	return await AxiosRequester.getInstance().checkEmailAvailability(email);
}

export const formatDateString = (dateString) => {
	const date = new Date(dateString);

	// Extract day, month, year, hours, and minutes
	const day = date.getDate().toString().padStart(2, '0');
	const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Months are zero-indexed
	const year = date.getFullYear();
	const hours = date.getHours().toString().padStart(2, '0');
	const minutes = date.getMinutes().toString().padStart(2, '0');

	// Format the date and time
	return `${day}/${month}/${year} ${hours}:${minutes}`;
}

export const formatDateTimeString = (dateTimeString) => {
	// Split the date and time parts
	const [datePart, timePart] = dateTimeString.split(' ');

	// Extract the hour and minute parts
	const [hour, minute] = timePart.split(':');

	// Combine the date part with the hour and minute parts
	return `${datePart} ${hour}:${minute}`;
}

export const isUsernameAvailable = async (username) => {
	return await AxiosRequester.getInstance().checkUsernameAvailability(username);
}
