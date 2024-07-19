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

export const isUsernameAvailable = async (username) => {
	return await AxiosRequester.getInstance().checkUsernameAvailability(username);
}
