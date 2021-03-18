const apiUrl = '/api/url';

const Game = (function(url){
    let configMap = {
        apiUrl: url
    };

    const privateInit = function (afterInit){
        console.log(configMap.apiUrl);
        setInterval(async () => {
            await Game.Model.getGameState();
        }, 2000);
        afterInit();
    }

    return {
        init: privateInit
    };

})('/api/url');

Game.Reversi = (function() {
    console.log('hallo, vanuit module');
})()

Game.Data = (function (){
    let configMap = {
        mock: [
            {
                url: '/api/Spel/Beurt/test',
                data: 0
            }
        ]
    };

    let stateMap = {
        environment: 'development'
    }

    const privateInit = function (env){
        if(env !== 'production' && env !== 'development') throw new Error("env is not in the correct format: expected 'production' or 'development'")
        stateMap.environment = env;
    }

    const get = async (url) => {
        if(stateMap.environment === 'production') {
            try {
                let response = await fetch(url);
                return response.text();
            } catch (e) {
                console.log(e.message);
            }
        } else {
            return getMockData(url);
        }
    }

    const getMockData = (url) => {
        const mockData = configMap.mock.find(mock => mock.url === url).data;
        return new Promise((resolve, reject) => resolve(mockData));
    }

    return {
        stateMap: stateMap,
        get: get,
        init: privateInit
    };
})()

Game.Model = (function (){
    let configMap = {
    };

    const privateInit = function (){
    }

    const _getGameState = async function(){
        let beurt = await Game.Data.get('/api/Spel/Beurt/test');
        if(beurt < 0 || beurt > 2) throw new Error("Unexpected value returned by Beurt endpoint: " + beurt);
        console.log('setting new gamestate');
        Game.Data.stateMap.gameState = beurt;
        console.dir(Game.Data.stateMap);
    }

    return {
        getGameState: _getGameState,
        init: privateInit
    };
})()