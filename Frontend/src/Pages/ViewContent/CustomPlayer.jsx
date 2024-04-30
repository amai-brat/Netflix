import shaka from "shaka-player";
import React, {useEffect} from "react";

export const CustomPlayer = ({videoUrl}) => {
  useEffect(() => {
    initApp()
  }, []);
  useEffect(() => {
    (async() => {
      if (!window.player) return;
      await window.player.load(videoUrl);
    })()
  }, [videoUrl]);
  return (
    <video id="video"
           width="1280px"
           height="720px"
           controls></video>
  );

  function initApp() {
    shaka.polyfill.installAll();
    if (shaka.Player.isBrowserSupported()) {
      initPlayer();
    } else {
      console.error('Browser not supported!');
    }
  }
  async function initPlayer() {
    const video = document.getElementById('video');
    const player = new shaka.Player();
    player.getNetworkingEngine().registerRequestFilter(function(type, request) {
      request.headers['Authorization'] = "Bearer " + sessionStorage.getItem("accessToken")
    });
    await player.attach(video);
    
    window.player = player;
    player.addEventListener('error', onErrorEvent);
    try {
      await player.load(videoUrl);
      console.log('The video has now been loaded!');
    } catch (e) {
      onError(e);
    }
  }
}
function onErrorEvent(event) {
  onError(event.detail);
}

function onError(error) {
  console.error('Error code', error.code, 'object', error);
}
