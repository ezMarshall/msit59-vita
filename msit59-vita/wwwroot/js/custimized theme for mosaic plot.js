/**
 * AnyChart is lightweight robust charting library with great API and Docs, that works with your stack and has tons of chart types and features.
 *
 * Theme: pastel
 * Version: 2.0.0 (2019-04-26)
 * License: https://www.anychart.com/buy/
 * Contact: sales@anychart.com
 * Copyright: AnyChart.com 2019. All rights reserved.
 */

// 根據此JS修改palette使其滿足Vita色階

(function () {
    "use strict";

    function a() {
        return window.anychart.color.setOpacity(this.sourceColor, 0.6, !0);
    }
    function b() {
        return window.anychart.color.darken(this.sourceColor);
    }
    function c() {
        return window.anychart.color.lighten(this.sourceColor);
    }
    var e = {
        palette: {
            type: "distinct",
            items: "#50e991 #ffb55a #90caf9 #fdcce5 #ffcc80 #ffab91 #f8bbd0 #d1c4e9 #9e9e9e #c7b299".split(
                " "
            )
        },
        defaultOrdinalColorScale: {
            autoColors: function (d) {
                return window.anychart.color.blendedHueProgression(
                    "#e6ee9c",
                    "#80cbc4",
                    d
                );
            }
        },
        defaultLinearColorScale: { colors: ["#e6ee9c", "#80cbc4"] },
        defaultFontSettings: { fontColor: "#9b8b7e" },
        defaultBackground: {
            fill: "#fefdfa",
            stroke: "#f1eeea",
            cornerType: "round",
            corners: 0
        },
        defaultTooltip: {
            fontSize: 12,
            background: { fill: "#eceff1 0.94", stroke: "1.5 #e3e3e3" },
            padding: { top: 8, right: 15, bottom: 10, left: 15 }
        },
        defaultColorRange: {
            stroke: "#bdbdbd",
            ticks: { stroke: "#bdbdbd", position: "outside", length: 7, enabled: !0 },
            minorTicks: {
                stroke: "#bdbdbd",
                position: "outside",
                length: 5,
                enabled: !0
            },
            marker: {
                padding: { top: 3, right: 3, bottom: 3, left: 3 },
                fill: "#37474f"
            }
        },
        defaultScroller: {
            fill: "#f2efec",
            selectedFill: "#dcd8d4",
            thumbs: {
                fill: "#F9FAFB",
                stroke: "#bdc8ce",
                hovered: { fill: "#bdc8ce", stroke: "#e9e4e4" }
            }
        },
        defaultAxis: {
            stroke: "#9b8b7e 0.4",
            ticks: { stroke: "#9b8b7e 0.4" },
            minorTicks: { stroke: "#9b8b7e 0.2" }
        },
        chart: {
            defaultSeriesSettings: {
                candlestick: {
                    normal: {
                        risingFill: "#90caf9",
                        risingStroke: "#90caf9",
                        fallingFill: "#ffcc80",
                        fallingStroke: "#ffcc80"
                    },
                    hovered: {
                        risingFill: c,
                        risingStroke: b,
                        fallingFill: c,
                        fallingStroke: b
                    },
                    selected: {
                        risingStroke: "3 #90caf9",
                        fallingStroke: "3 #ffcc80",
                        risingFill: "#333333 0.85",
                        fallingFill: "#333333 0.85"
                    }
                },
                ohlc: {
                    normal: { risingStroke: "#90caf9", fallingStroke: "#ffcc80" },
                    hovered: { risingStroke: b, fallingStroke: b },
                    selected: { risingStroke: "3 #90caf9", fallingStroke: "3 #ffcc80" }
                }
            },
            padding: { top: 20, right: 25, bottom: 15, left: 15 }
        },
        defaultGridSettings: { stroke: "#9b8b7e 0.4" },
        defaultMinorGridSettings: { stroke: "#9b8b7e 0.2" },
        defaultSeparator: { fill: "#9b8b7e 0.4" },
        map: {
            unboundRegions: { enabled: !0, fill: "#f2efec", stroke: "#dcd8d4" },
            defaultSeriesSettings: {
                base: { normal: { stroke: b, labels: { fontColor: "#212121" } } },
                connector: {
                    normal: {
                        stroke: "1.5 #90caf9",
                        markers: { stroke: "1.5 #e9e6e3", fill: "#80cbc4" }
                    },
                    hovered: { stroke: "1.5 #37474f", markers: { fill: "#80cbc4" } },
                    selected: { stroke: "1.5 #000", markers: { fill: "#000" } }
                }
            }
        },
        sparkline: {
            padding: 0,
            background: { stroke: "#fefdfa" },
            defaultSeriesSettings: {
                area: { stroke: "1.5 #90caf9", fill: "#90caf9 0.5" },
                column: { fill: "#90caf9", negativeFill: "#ffcc80" },
                line: { stroke: "1.5 #90caf9" },
                winLoss: { fill: "#90caf9", negativeFill: "#ffcc80" }
            }
        },
        bullet: {
            background: { stroke: "#fefdfa" },
            defaultMarkerSettings: { fill: "#90caf9", stroke: "2 #90caf9" },
            padding: { top: 5, right: 10, bottom: 5, left: 10 },
            margin: { top: 0, right: 0, bottom: 0, left: 0 },
            rangePalette: {
                items: ["#CDCDCC", "#D7D7D5", "#E4E3E2", "#EEEDEB", "#F8F7F4"]
            }
        },
        heatMap: {
            normal: { stroke: "1 #fefdfa", labels: { fontColor: "#212121" } },
            hovered: { stroke: "1.5 #fefdfa" },
            selected: { stroke: "2 #fefdfa" }
        },
        treeMap: {
            normal: {
                headers: {
                    background: { enabled: !0, fill: "#f2efec", stroke: "#dcd8d4" }
                },
                labels: { fontColor: "#212121" },
                stroke: "#dcd8d4"
            },
            hovered: {
                headers: {
                    fontColor: "#757575",
                    background: { fill: "#dcd8d4", stroke: "#dcd8d4" }
                }
            },
            selected: { labels: { fontColor: "#9b8b7e" }, stroke: "2 #eceff1" }
        },
        stock: {
            padding: [20, 30, 20, 60],
            defaultPlotSettings: {
                xAxis: { background: { fill: "#f2efec 0.5", stroke: "#dcd8d4" } }
            },
            scroller: {
                fill: "none",
                selectedFill: "#f2efec 0.5",
                outlineStroke: "#dcd8d4",
                defaultSeriesSettings: {
                    base: { selected: { stroke: a } },
                    candlestick: {
                        normal: {
                            risingFill: "#999",
                            risingStroke: "#999",
                            fallingFill: "#999",
                            fallingStroke: "#999"
                        },
                        selected: {
                            risingStroke: a,
                            fallingStroke: a,
                            risingFill: a,
                            fallingFill: a
                        }
                    },
                    ohlc: {
                        normal: { risingStroke: "#999", fallingStroke: "#999" },
                        selected: { risingStroke: a, fallingStroke: a }
                    }
                }
            },
            xAxis: { background: { enabled: !1 } }
        }
    };
    window.anychart = window.anychart || {};
    window.anychart.themes = window.anychart.themes || {};
    window.anychart.themes.customized = e;
})();