import axios from "axios";
import { getBase64 } from "../utils/utils";

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
		console.log("Réponse de l'API :", { response: response.data });
		return response.data;
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};
/*
 * registerRequest:
 * @param {object} data:
 * @param {string} data.userName
 * @param {string} data.email
 * @param {string} data.password
 */
export const registerRequest = async (data) => {
	const apiUrl = baseUrl + "User/register";
	try {
		const response = await axios.post(apiUrl, data, config);
		console.log("Réponse de l'API :", { response: response.data });
		return response.data;
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};

/* postScript:
 * @param {object} data:
 * @param {string} data.ScriptName
 * @param {string} data.Description
 * @param {string} data.ProgrammingLanguage
 * @param {string} data.InputScryptType
 * @param {string} data.OutputScryptType
 * @param {string} data.Visibility
 * @param {string} data.File
 * @param {string} data.UserId
 */
export const postScript = async (data) => {
	const apiUrl = baseUrl + "Script/upload";
	const dataUpdated = data;
	// console.log(data.SourceScriptBinary);
	const fileAsBase64 = await getBase64(data.SourceScriptBinary);
	dataUpdated.SourceScriptBinary = fileAsBase64;
	// console.log(dataUpdated);
	try {
		const response = await axios.post(apiUrl, data, config);
		console.log("Réponse de l'API :", { response: response.data });
		return response.data;
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};

/* postScriptVersion:
 * @param {object} data:
 * @param {string} data.ScriptId
 * @param {string} data.VersionNumber
 * @param {string} data.Comment
 * @param {string} data.SourceScriptBinary
 * @param {string} data.CreatorUserId
 */
export const postScriptVersion = async (data) => {
	const apiUrl = baseUrl + "Script/upload/version";
	const dataUpdated = data;
	let fileAsBase64;
	if (typeof data.SourceScriptBinary === "string") {
		fileAsBase64 = btoa(unescape(encodeURIComponent(data.SourceScriptBinary)));
	} else {
		fileAsBase64 = await getBase64(data.SourceScriptBinary);
	}
	dataUpdated.SourceScriptBinary = fileAsBase64;
	// console.log(dataUpdated);
	try {
		const response = await axios.post(apiUrl, data, config);
		console.log("Réponse de l'API :", { response: response.data });
		return "success";
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};

/* getScriptById:
 * @param {string} scriptId
 */
export const getScriptById = async (scriptId) => {
	const apiUrl = baseUrl + `Script/${scriptId}`;
	try {
		const response = await axios.get(apiUrl);
		console.log("Réponse de l'API :", { response: response.data });
		return response.data;
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};

/* getScripts:
 * @param {string} userId
 */
export const getScripts = async (userId) => {
	const apiUrl = baseUrl + `Script/user/${userId}`;
	try {
		const response = await axios.get(apiUrl);
		console.log("Réponse de l'API :", { response: response.data });
		return response.data;
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};

/* deleteScript: Delete the script and all its versions from DB
 * @param {string} scriptId
 */
export const deleteScript = async (scriptId) => {
	const apiUrl = baseUrl + `Script/deleteFull/${scriptId}`;
	try {
		const response = await axios.delete(apiUrl);
		console.log("Réponse de l'API :", { response: response.data });
		return response.data;
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};

/* deleteScriptVersion: Delete the script version whose Id is provided from DB
 * @param {string} scriptVersionId
 */
export const deleteScriptVersion = async (scriptVersionId) => {
	const apiUrl = baseUrl + `Script/delete/version/${scriptVersionId}`;
	try {
		const response = await axios.delete(apiUrl);
		console.log("Réponse de l'API :", { response: response.data });
		return response.data;
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};

/* getScriptBlob: Get the script blob from DB
 * @param {string} scriptId
 * @return {object} response.data
 * @return {string} response.data.blob (base64)
 * @return {string} response.data.fileName
 */
export const getScriptBlob = async (scriptId, userId) => {
	const apiUrl = baseUrl + `Script/${userId}/${scriptId}/blob`;
	try {
		const response = await axios.get(apiUrl);
		console.log("Réponse de l'API :", { response: response.data });
		return response.data;
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};

/* getScriptVersionBlob: Get the script blob from DB
 * @param {string} scriptVersionId
 * @return {object} response.data
 * @return {string} response.data.blob (base64)
 * @return {string} response.data.fileName
 */
export const getScriptVersionBlob = async (scriptVersionId, userId) => {
	const apiUrl = baseUrl + `Script/${userId}/${scriptVersionId}/versionBlob`;
	try {
		const response = await axios.get(apiUrl);
		console.log("Réponse de l'API :", { response: response.data });
		return response.data;
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};

/* getScriptVersions: Get all versions of a script
 * @param {string} scriptId
 * @return {array} array of script versions
 */
export const getScriptVersions = (scriptId) => {
	const apiUrl = baseUrl + `Script/versions/${scriptId}`;
	return new Promise((resolve, reject) => {
		try {
			return axios.get(apiUrl).then((response) => {
				console.log("Réponse de l'API :", { response: response.data });
				return resolve(response.data);
			});
		} catch (error) {
			console.error("Erreur lors de la requête :", error);
			return reject(error);
		}
	});
};

export const searchScriptsByKeyWord = async (keyWord) => {
	const apiUrl = baseUrl + `Script/search/${keyWord}`;
	try {
		const response = await axios.get(apiUrl);
		console.log("Réponse de l'API :", { response: response.data });
		return response.data;
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};

export const updateScript = async (data) => {
	const apiUrl = baseUrl + `Script/update`;
	try {
		const response = await axios.put(apiUrl, data, config);
		console.log("Réponse de l'API :", { response });
		if (response.status === 200) {
			return data;
		} else {
			return null;
		}
	} catch (error) {
		console.error("Erreur lors de la requête :", error);
	}
};
