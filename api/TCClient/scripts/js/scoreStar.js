window.onload = function () {
  var starModule = (function () {

    var oStarOut = document.querySelector('.star-out');
    var oStarScore = document.querySelector('.star-score');
    var totalWidth = 2.5; // 星级评分组件总宽度
    var fraction = 0.0; // 分数

    var bind = function (obj, ev, fn) {
      if (obj.addEventListener) {
        obj.addEventListener(ev, fn, false);
      } else {
        obj.attachEvent('on' + ev, function() {
          fn.call(obj);
        });
      }
    };

    var star = bind(oStarOut, 'touchend', function (e) {
      // 鼠标点与星级评分组件左端的距离
        var oDis = e.changedTouches[0].pageX - e.srcElement.offsetLeft;
        oDis = oDis / 50;
        console.log(oDis);
        var newScore = (oDis / totalWidth).toFixed(2);
        newScore = newScore > 1 ? 1 : newScore;
      if (newScore > 0.00 && newScore <= 0.20) {
        newScore = 0.20;
        fraction = (5 * newScore).toFixed(1); // 1.0分
      }

      if (newScore > 0.20 && newScore <= 0.40) {
        newScore = 0.40;
        fraction = (5 * newScore).toFixed(1); // 2.0分
      }

      if (newScore > 0.40 && newScore <= 0.60) {
        newScore = 0.60;
        fraction = (5 * newScore).toFixed(1); // 3.0分
      }

      if (newScore > 0.60 && newScore <= 0.80) {
        newScore = 0.80;
        fraction = (5 * newScore).toFixed(1); // 4.0分
      }

      if (newScore > 0.80 && newScore <= 1) {
        newScore = 1;
        fraction = (5 * newScore).toFixed(1); // 5.0分
      }

        var starWidth = (newScore * totalWidth).toFixed(2);
        starWidth = starWidth > 2.5 ? 2.5 : starWidth;

      oStarScore.style.width = starWidth + 'rem';
      oStarOut.setAttribute('data-score', fraction);
    });

    return {
      star: star
    };

  })();

  starModule.star;

};