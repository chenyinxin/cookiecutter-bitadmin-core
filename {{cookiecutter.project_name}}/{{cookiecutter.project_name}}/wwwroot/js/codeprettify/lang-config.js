// Copyright (C) 2012 Kyo Nagashima <kyo@hail2u.net>
//
// LICENSE: http://hail2u.mit-license.org/2012



/**
 * @fileoverview
 * Registers a language handler for general config file.
 *
 *
 * To use, include prettify.js and this file in your HTML page.
 * Then put your code in an HTML tag like
 *      <pre class="prettyprint lang-config"></pre>
 *
 *
 * @author kyo@hail2u.net
 */

PR['registerLangHandler'](
  PR['createSimpleLexer'](
    [
      [PR['PR_COMMENT'], /^#[^\r\n]*/, null, '#'],
      [PR['PR_PLAIN'], /^\s+/, null, ' \t\r\n']
    ],
    [
      [PR['PR_PLAIN'], /^\w+/]
    ]), ['config', 'conf']);
