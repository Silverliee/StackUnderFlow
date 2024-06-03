import axios from "axios";

const baseUrl = "http://localhost:5008/";
const config = {
	headers: {
		"Content-Type": "application/json",
		accept: "*/*",
	},
};

/*
 * loginRequest:
 * @param {object} data:
 * @param {string} data.email
 * @param {string} data.password
 * @returns {object} response.data
 * @returns {string} response.data.token
 * @returns {object} response.data.user
 */
export const loginRequest = async (data) => {
	const apiUrl = baseUrl + "User/login";
	try {
		const response = await axios.post(apiUrl, data, config);
		console.log("Réponse de l'API :", response.data);
		return response.data;
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};
/*
 * register:
 * @param {object} data:
 * @param {string} data.userName
 * @param {string} data.email
 * @param {string} data.password
 */
export const register = async (data) => {
	const apiUrl = baseUrl + "User/register";
	try {
		const response = await axios.post(apiUrl, data, config);
		console.log("Réponse de l'API :", response.data);
		return response.data;
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};
