class SocketManager {
    _instance = null;
    constructor() {}
    socket = null;

    get instance() {
        if (!this._instance) {
            this._instance = new SocketManager();
        }
        return this._instance;
    }

    connectWebSocketPipeline(pipelineId) {
        const url = `ws://localhost:5008/subscribe/${pipelineId}`;
        this.socket = new WebSocket(url);

        this.socket.onopen = () => {
            console.log('WebSocket connected');
        };

        this.socket.onmessage = (event) => {
            console.log('Message from server:', event.data);
            // Traitez ici les messages reçus du serveur
        };

        this.socket.onclose = (event) => {
            console.log('WebSocket closed:', event);
        };

        this.socket.onerror = (error) => {
            console.error('WebSocket error:', error);
        };
    }

}