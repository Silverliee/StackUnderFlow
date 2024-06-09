import axios from "axios";
import { getBase64 } from "../utils/utils";

class AxiosRequester {
	token = null;
	baseUrl = "http://localhost:5008/";
	_instance = null;

	constructor() {}

	setToken(token) {
		this.token = token;
	}

	static getInstance() {
		if (!AxiosRequester._instance) {
			AxiosRequester._instance = new AxiosRequester();
		}
		return AxiosRequester._instance;
	}

	getConfig() {
		return {
			headers: {
				"Content-Type": "application/json",
				Authorization: `Bearer ${this.token}`,
				accept: "*/*",
			},
		};
	}

	/*
	 * loginRequest:
	 * @param {object} data:
	 * @param {string} data.email
	 * @param {string} data.password
	 * @returns {object} response.data
	 * @returns {string} response.data.token
	 * @returns {object} response.data.user
	 */
	loginRequest = async (data) => {
		const apiUrl = this.baseUrl + "User/login";
		try {
			const response = await axios.post(apiUrl, data, this.getConfig());
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
	registerRequest = async (data) => {
		const apiUrl = this.baseUrl + "User/register";
		try {
			const response = await axios.post(apiUrl, data, this.getConfig());
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
	postScript = async (data) => {
		const apiUrl = this.baseUrl + "Script";
		const dataUpdated = data;
		// console.log(data.SourceScriptBinary);
		const fileAsBase64 = await getBase64(data.SourceScriptBinary);
		dataUpdated.SourceScriptBinary = fileAsBase64;
		// console.log(dataUpdated);
		try {
			const response = await axios.post(apiUrl, data, this.getConfig());
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
	postScriptVersion = async (data) => {
		const apiUrl = this.baseUrl + "Script/version";
		const dataUpdated = data;
		let fileAsBase64;
		if (typeof data.SourceScriptBinary === "string") {
			fileAsBase64 = btoa(
				unescape(encodeURIComponent(data.SourceScriptBinary))
			);
		} else {
			fileAsBase64 = await getBase64(data.SourceScriptBinary);
		}
		dataUpdated.SourceScriptBinary = fileAsBase64;
		// console.log(dataUpdated);
		try {
			const response = await axios.post(apiUrl, data, this.getConfig());
			console.log("Réponse de l'API :", { response: response.data });
			return "success";
		} catch (error) {
			console.error("Erreur lors de la requête :", error);
		}
	};

	/* getScriptById:
	 * @param {string} scriptId
	 */
	getScriptById = async (scriptId) => {
		const apiUrl = this.baseUrl + `Script/${scriptId}`;
		try {
			const response = await axios.get(apiUrl, this.getConfig());
			console.log("Réponse de l'API :", { response: response.data });
			return response.data;
		} catch (error) {
			console.error("Erreur lors de la requête :", error);
		}
	};

	/* getScripts:
	 * @param {string} userId
	 */
	getScripts = async () => {
		const apiUrl = this.baseUrl + `Script/user`;
		try {
			const response = await axios.get(apiUrl, this.getConfig());
			console.log("Réponse de l'API :", { response: response.data });
			return response.data;
		} catch (error) {
			console.error("Erreur lors de la requête :", error);
		}
	};

	/* deleteScript: Delete the script and all its versions from DB
	 * @param {string} scriptId
	 */
	deleteScript = async (scriptId) => {
		const apiUrl = this.baseUrl + `Script/${scriptId}`;
		try {
			const response = await axios.delete(apiUrl, this.getConfig());
			console.log("Réponse de l'API :", { response: response.data });
			return response.data;
		} catch (error) {
			console.error("Erreur lors de la requête :", error);
		}
	};

	/* deleteScriptVersion: Delete the script version whose Id is provided from DB
	 * @param {string} scriptVersionId
	 */
	deleteScriptVersion = async (scriptVersionId) => {
		const apiUrl = this.baseUrl + `Script/version/${scriptVersionId}`;
		try {
			const response = await axios.delete(apiUrl, this.getConfig());
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
	getScriptBlob = async (scriptId) => {
		const apiUrl = this.baseUrl + `Script/${scriptId}/file`;
		try {
			const response = await axios.get(apiUrl, this.getConfig());
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
	getScriptVersionBlob = async (scriptVersionId) => {
		const apiUrl = this.baseUrl + `Script/version/${scriptVersionId}/file`;
		try {
			const response = await axios.get(apiUrl, this.getConfig());
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
	getScriptVersions = (scriptId) => {
		const apiUrl = this.baseUrl + `Script/${scriptId}/versions`;
		return new Promise((resolve, reject) => {
			try {
				return axios.get(apiUrl, this.getConfig()).then((response) => {
					console.log("Réponse de l'API :", { response: response.data });
					return resolve(response.data);
				});
			} catch (error) {
				console.error("Erreur lors de la requête :", error);
				return reject(error);
			}
		});
	};

	searchScriptsByKeyWord = async (keyWord) => {
		const apiUrl = this.baseUrl + `Script/search/${keyWord}`;
		try {
			const response = await axios.get(apiUrl, this.getConfig());
			console.log("Réponse de l'API :", { response: response.data });
			return response.data;
		} catch (error) {
			console.error("Erreur lors de la requête :", error);
		}
	};

	updateScript = async (data) => {
		const apiUrl = this.baseUrl + `Script`;
		try {
			const response = await axios.put(apiUrl, data, this.getConfig());
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

	getUserByToken = async () => {
		const apiUrl = this.baseUrl + `User`;
		console.log(this.getConfig());
		try {
			const response = await axios.get(apiUrl, this.getConfig());
			console.log("Réponse de l'API :", { response: response });
			if (response.status === 200) {
				return response.data;
			} else {
				return null;
			}
		} catch (error) {
			console.error("Erreur lors de la requête :", error);
		}
	};
}

export default AxiosRequester;
