/*{
  "DESCRIPTION": "A zoom-and-flash pump locked to the music's tempo. Set the BPM (get it from the beat map) and every beat lands a punch \u2014 no keyframing.",
  "CATEGORIES": ["Guillotine", "Distortion"],
  "INPUTS": [
  {
    "NAME": "inputImage",
    "TYPE": "image"
  },
  {
    "NAME": "bpm",
    "TYPE": "float",
    "DEFAULT": 120.0,
    "MIN": 60.0,
    "MAX": 200.0
  },
  {
    "NAME": "beatOffset",
    "TYPE": "float",
    "DEFAULT": 0.0,
    "MIN": 0.0,
    "MAX": 2.0
  },
  {
    "NAME": "intensity",
    "TYPE": "float",
    "DEFAULT": 0.5,
    "MIN": 0.0,
    "MAX": 1.0
  }
]
}*/
void main() {
  vec2 uv = isf_FragNormCoord;

  // Beats are periodic, so the shader needs the TEMPO, not the waveform: TIME plus a BPM gives
  // the exact beat phase. beatOffset shifts the grid to line up with the first downbeat.
  float beats = max(TIME - beatOffset, 0.0) * bpm / 60.0;
  float phase = fract(beats);

  // Sharp attack, exponential decay — a percussive pump rather than a smooth sine.
  float pump = exp(-phase * 6.0) * intensity;

  // Punch in toward centre on each beat.
  vec2 centered = uv - vec2(0.5);
  vec2 zoomed = centered / (1.0 + pump * 0.06) + vec2(0.5);

  vec4 c = IMG_NORM_PIXEL(inputImage, zoomed);

  // A brightness lift on the same envelope so the hit reads even on busy footage.
  gl_FragColor = vec4(c.rgb * (1.0 + pump * 0.25), c.a);
}
